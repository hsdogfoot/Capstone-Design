#pragma once
#include "Base.h"

class GraphicDevice final : public Base
{
public:
	static bool Create(GraphicDevice** outInstance, HWND windowHandle, UINT windowWidth, UINT windowHeight);

	void RenderFrame() const;

	bool Resize(UINT windowWidth, UINT windowHeight);

	inline ID3D11Device* GetDevice() const;
	inline ID3D11DeviceContext* GetDeviceContext() const;

private:
	GraphicDevice();
	virtual ~GraphicDevice() override = default;

	bool initialize(HWND windowHandle, UINT windowWidth, UINT windowHeight);
	bool initializeDirectX(HWND windowHandle, UINT windowWidth, UINT windowHeight);

	virtual void destroy() override;

private:
	ID3D11Device* mDevice;
	ID3D11DeviceContext* mDeviceContext;

	IDXGISwapChain* mSwapChain;
	ID3D11RenderTargetView* mRenderTargetView;

	ID3D11DepthStencilView* mDepthStencilView;
	ID3D11Texture2D* mDepthStencilBuffer;
	ID3D11DepthStencilState* mDepthStencilState;

	ID3D11RasterizerState* mRasterizerState;
	ID3D11SamplerState* mSamplerState;
	ID3D11BlendState* mBlendState;
};

inline ID3D11Device* GraphicDevice::GetDevice() const
{
	return mDevice;
}

inline ID3D11DeviceContext* GraphicDevice::GetDeviceContext() const
{
	return mDeviceContext;
}