#include "stdafx.h"
#include "GraphicDevice.h"

#include "AdapterReader.h"

bool GraphicDevice::Create(GraphicDevice** outInstance, HWND windowHandle, UINT windowWidth, UINT windowHeight)
{
	Assert(windowHandle != nullptr);

	GraphicDevice* instance = new GraphicDevice();
	if (!instance->initialize(windowHandle, windowWidth, windowHeight))
	{
		instance->Release();

		return false;
	}

	*outInstance = instance;

	return true;
}

void GraphicDevice::RenderFrame() const
{
	float backgroundColor[] = { 0.5f, 0.5f, 0.55f, 0.0f };
	mDeviceContext->ClearRenderTargetView(mRenderTargetView, backgroundColor);
	mDeviceContext->ClearDepthStencilView(mDepthStencilView, D3D11_CLEAR_DEPTH | D3D11_CLEAR_STENCIL, 1.0f, 0);

	mSwapChain->Present(1, 0);
}

bool GraphicDevice::Resize(UINT windowWidth, UINT windowHeight)
{
	ReleaseCOM(mRenderTargetView);
	ReleaseCOM(mDepthStencilView);
	ReleaseCOM(mDepthStencilBuffer);

	HRESULT hResult = mSwapChain->ResizeBuffers(1, windowWidth, windowHeight, DXGI_FORMAT_R8G8B8A8_UNORM, DXGI_SWAP_CHAIN_FLAG_ALLOW_MODE_SWITCH);
	if (FAILED(hResult))
	{
		HRLog(hResult);

		return false;
	}

	ID3D11Texture2D* backBuffer;
	hResult = mSwapChain->GetBuffer(0, __uuidof(ID3D11Texture2D), reinterpret_cast<void**>(&backBuffer));
	if (FAILED(hResult))
	{
		HRLog(hResult);

		return false;
	}

	hResult = mDevice->CreateRenderTargetView(backBuffer, nullptr, &mRenderTargetView);
	if (FAILED(hResult))
	{
		HRLog(hResult);

		return false;
	}

	ReleaseCOM(backBuffer);

	D3D11_TEXTURE2D_DESC textureDesc;
	ZeroMemory(&textureDesc, sizeof(D3D11_TEXTURE2D_DESC));

	textureDesc.Width = windowWidth;
	textureDesc.Height = windowHeight;
	textureDesc.MipLevels = 1;
	textureDesc.ArraySize = 1;
	textureDesc.Format = DXGI_FORMAT::DXGI_FORMAT_D24_UNORM_S8_UINT;
	textureDesc.SampleDesc.Count = 1;
	textureDesc.SampleDesc.Quality = 0;
	textureDesc.Usage = D3D11_USAGE::D3D11_USAGE_DEFAULT;
	textureDesc.BindFlags = D3D11_BIND_FLAG::D3D11_BIND_DEPTH_STENCIL;
	textureDesc.CPUAccessFlags = 0;
	textureDesc.MiscFlags = 0;

	hResult = mDevice->CreateTexture2D(&textureDesc, nullptr, &mDepthStencilBuffer);
	if (FAILED(hResult))
	{
		HRLog(hResult);

		return false;
	}

	hResult = mDevice->CreateDepthStencilView(mDepthStencilBuffer, nullptr, &mDepthStencilView);
	if (FAILED(hResult))
	{
		HRLog(hResult);

		return false;
	}

	mDeviceContext->OMSetRenderTargets(1, &mRenderTargetView, mDepthStencilView);

	D3D11_VIEWPORT viewport;
	ZeroMemory(&viewport, sizeof(D3D11_VIEWPORT));

	viewport.TopLeftX = 0;
	viewport.TopLeftY = 0;
	viewport.Width = static_cast<float>(windowWidth);
	viewport.Height = static_cast<float>(windowHeight);
	viewport.MinDepth = 0.0f;
	viewport.MaxDepth = 1.0f;

	mDeviceContext->RSSetViewports(1, &viewport);

	return true;
}

GraphicDevice::GraphicDevice()
	: Base()
	, mDevice(nullptr)
	, mDeviceContext(nullptr)
	, mSwapChain(nullptr)
	, mRenderTargetView(nullptr)
	, mDepthStencilView(nullptr)
	, mDepthStencilBuffer(nullptr)
	, mDepthStencilState(nullptr)
	, mRasterizerState(nullptr)
	, mSamplerState(nullptr)
	, mBlendState(nullptr)
{
}

bool GraphicDevice::initialize(HWND windowHandle, UINT windowWidth, UINT windowHeight)
{
	HRESULT hResult = CoInitialize(nullptr);
	if (FAILED(hResult))
	{
		HRLog(hResult);

		return false;
	}

	if (!initializeDirectX(windowHandle, windowWidth, windowHeight))
	{
		return false;
	}

	return true;
}

bool GraphicDevice::initializeDirectX(HWND windowHandle, UINT windowWidth, UINT windowHeight)
{
	std::vector<AdapterData> adapters = AdapterReader::GetAdapters();
	if (adapters.empty())
	{
		ErrorLog("GetAdapters failed.");

		return false;
	}

	UINT createFlags = 0;
#if defined(DEBUG) || defined(_DEBUG)
	createFlags |= D3D11_CREATE_DEVICE_DEBUG;
#endif

	DXGI_SWAP_CHAIN_DESC swapChainDesc;
	ZeroMemory(&swapChainDesc, sizeof(DXGI_SWAP_CHAIN_DESC));

	swapChainDesc.BufferDesc.Width = windowWidth;
	swapChainDesc.BufferDesc.Height = windowHeight;
	swapChainDesc.BufferDesc.RefreshRate.Numerator = 60;
	swapChainDesc.BufferDesc.RefreshRate.Denominator = 1;
	swapChainDesc.BufferDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
	swapChainDesc.BufferDesc.ScanlineOrdering = DXGI_MODE_SCANLINE_ORDER_UNSPECIFIED;
	swapChainDesc.BufferDesc.Scaling = DXGI_MODE_SCALING_UNSPECIFIED;

	swapChainDesc.SampleDesc.Count = 1;
	swapChainDesc.SampleDesc.Quality = 0;

	swapChainDesc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
	swapChainDesc.BufferCount = 1;
	swapChainDesc.OutputWindow = windowHandle;
	swapChainDesc.Windowed = TRUE;
	swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT_DISCARD;
	swapChainDesc.Flags = DXGI_SWAP_CHAIN_FLAG_ALLOW_MODE_SWITCH;

	HRESULT hResult = D3D11CreateDeviceAndSwapChain(adapters[0].Adapter, D3D_DRIVER_TYPE_UNKNOWN, nullptr, createFlags, nullptr, 0, D3D11_SDK_VERSION, &swapChainDesc, &mSwapChain, &mDevice, nullptr, &mDeviceContext);
	if (FAILED(hResult))
	{
		HRLog(hResult);

		return false;
	}

	ID3D11Texture2D* backBuffer;
	hResult = mSwapChain->GetBuffer(0, __uuidof(ID3D11Texture2D), reinterpret_cast<void**>(&backBuffer));
	if (FAILED(hResult))
	{
		HRLog(hResult);

		return false;
	}

	hResult = mDevice->CreateRenderTargetView(backBuffer, nullptr, &mRenderTargetView);
	if (FAILED(hResult))
	{
		HRLog(hResult);

		return false;
	}

	ReleaseCOM(backBuffer);

	D3D11_TEXTURE2D_DESC textureDesc;
	ZeroMemory(&textureDesc, sizeof(D3D11_TEXTURE2D_DESC));

	textureDesc.Width = windowWidth;
	textureDesc.Height = windowHeight;
	textureDesc.MipLevels = 1;
	textureDesc.ArraySize = 1;
	textureDesc.Format = DXGI_FORMAT::DXGI_FORMAT_D24_UNORM_S8_UINT;
	textureDesc.SampleDesc.Count = 1;
	textureDesc.SampleDesc.Quality = 0;
	textureDesc.Usage = D3D11_USAGE::D3D11_USAGE_DEFAULT;
	textureDesc.BindFlags = D3D11_BIND_FLAG::D3D11_BIND_DEPTH_STENCIL;
	textureDesc.CPUAccessFlags = 0;
	textureDesc.MiscFlags = 0;

	hResult = mDevice->CreateTexture2D(&textureDesc, nullptr, &mDepthStencilBuffer);
	if (FAILED(hResult))
	{
		HRLog(hResult);

		return false;
	}

	hResult = mDevice->CreateDepthStencilView(mDepthStencilBuffer, nullptr, &mDepthStencilView);
	if (FAILED(hResult))
	{
		HRLog(hResult);

		return false;
	}

	mDeviceContext->OMSetRenderTargets(1, &mRenderTargetView, mDepthStencilView);

	D3D11_DEPTH_STENCIL_DESC depthStencilDesc;
	ZeroMemory(&depthStencilDesc, sizeof(D3D11_DEPTH_STENCIL_DESC));

	depthStencilDesc.DepthEnable = true;
	depthStencilDesc.DepthWriteMask = D3D11_DEPTH_WRITE_MASK::D3D11_DEPTH_WRITE_MASK_ALL;
	depthStencilDesc.DepthFunc = D3D11_COMPARISON_FUNC::D3D11_COMPARISON_LESS_EQUAL;

	hResult = mDevice->CreateDepthStencilState(&depthStencilDesc, &mDepthStencilState);
	if (FAILED(hResult))
	{
		HRLog(hResult);

		return false;
	}

	D3D11_VIEWPORT viewport;
	ZeroMemory(&viewport, sizeof(D3D11_VIEWPORT));

	viewport.TopLeftX = 0;
	viewport.TopLeftY = 0;
	viewport.Width = static_cast<float>(windowWidth);
	viewport.Height = static_cast<float>(windowHeight);
	viewport.MinDepth = 0.0f;
	viewport.MaxDepth = 1.0f;

	mDeviceContext->RSSetViewports(1, &viewport);

	D3D11_RASTERIZER_DESC rasterizerDesc;
	ZeroMemory(&rasterizerDesc, sizeof(D3D11_RASTERIZER_DESC));

	rasterizerDesc.FillMode = D3D11_FILL_MODE::D3D11_FILL_SOLID;
	rasterizerDesc.CullMode = D3D11_CULL_MODE::D3D11_CULL_BACK;
	hResult = mDevice->CreateRasterizerState(&rasterizerDesc, &mRasterizerState);
	if (FAILED(hResult))
	{
		HRLog(hResult);

		return false;
	}

	D3D11_SAMPLER_DESC samplerDesc;
	ZeroMemory(&samplerDesc, sizeof(D3D11_SAMPLER_DESC));

	samplerDesc.Filter = D3D11_FILTER_MIN_MAG_MIP_LINEAR;
	samplerDesc.AddressU = D3D11_TEXTURE_ADDRESS_WRAP;
	samplerDesc.AddressV = D3D11_TEXTURE_ADDRESS_WRAP;
	samplerDesc.AddressW = D3D11_TEXTURE_ADDRESS_WRAP;
	samplerDesc.ComparisonFunc = D3D11_COMPARISON_NEVER;
	samplerDesc.MinLOD = 0.0f;
	samplerDesc.MaxLOD = D3D11_FLOAT32_MAX;

	hResult = mDevice->CreateSamplerState(&samplerDesc, &mSamplerState);
	if (FAILED(hResult))
	{
		HRLog(hResult);

		return false;
	}

	D3D11_BLEND_DESC blendDesc;
	ZeroMemory(&blendDesc, sizeof(D3D11_BLEND_DESC));

	blendDesc.AlphaToCoverageEnable = FALSE;
	blendDesc.IndependentBlendEnable = FALSE;
	blendDesc.RenderTarget[0].BlendEnable = FALSE;
	blendDesc.RenderTarget[0].SrcBlend = D3D11_BLEND_SRC_ALPHA;
	blendDesc.RenderTarget[0].SrcBlendAlpha = D3D11_BLEND_ONE;
	blendDesc.RenderTarget[0].DestBlend = D3D11_BLEND_INV_SRC_ALPHA;
	blendDesc.RenderTarget[0].DestBlendAlpha = D3D11_BLEND_ZERO;
	blendDesc.RenderTarget[0].BlendOp = D3D11_BLEND_OP_ADD;
	blendDesc.RenderTarget[0].BlendOpAlpha = D3D11_BLEND_OP_ADD;
	blendDesc.RenderTarget[0].RenderTargetWriteMask = D3D11_COLOR_WRITE_ENABLE_ALL;

	hResult = mDevice->CreateBlendState(&blendDesc, &mBlendState);
	if (FAILED(hResult))
	{
		HRLog(hResult);

		return false;
	}

	return true;
}

void GraphicDevice::destroy()
{
	ReleaseCOM(mDevice);
	ReleaseCOM(mDeviceContext);

	ReleaseCOM(mSwapChain);
	ReleaseCOM(mRenderTargetView);

	ReleaseCOM(mDepthStencilView);
	ReleaseCOM(mDepthStencilBuffer);
	ReleaseCOM(mDepthStencilState);

	ReleaseCOM(mRasterizerState);
	ReleaseCOM(mSamplerState);
	ReleaseCOM(mBlendState);

	for (auto adapterData : AdapterReader::GetAdapters())
	{
		ReleaseCOM(adapterData.Adapter);
	}
}
