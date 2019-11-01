#include "stdafx.h"
#include "RenderWindow.h"

#include "Application.h"

LRESULT CALLBACK HandleMsgRedirect(HWND hWND, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	if (uMsg == WM_CLOSE)
	{
		DestroyWindow(hWND);

		return 0;
	}

	Application* const application = reinterpret_cast<Application*>(GetWindowLongPtr(hWND, GWLP_USERDATA));

	return application->WindowProc(hWND, uMsg, wParam, lParam);
}

LRESULT CALLBACK HandleMessageSetup(HWND hWND, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	if (uMsg == WM_NCCREATE)
	{
		const CREATESTRUCTW* const pCreate = reinterpret_cast<CREATESTRUCTW*>(lParam);
		Application* application = reinterpret_cast<Application*>(pCreate->lpCreateParams);

		SetWindowLongPtr(hWND, GWLP_USERDATA, reinterpret_cast<LONG_PTR>(application));
		SetWindowLongPtr(hWND, GWLP_WNDPROC, reinterpret_cast<LONG_PTR>(HandleMsgRedirect));

		return application->WindowProc(hWND, uMsg, wParam, lParam);
	}

	return DefWindowProc(hWND, uMsg, wParam, lParam);
}

bool RenderWindow::Create(RenderWindow** outInstance, RenderWindowDesc& desc, Application* application)
{
	Assert(application != nullptr);

	RenderWindow* instance = new RenderWindow(desc);
	if (!instance->initialize(application))
	{
		instance->Release();

		return false;
	}

	*outInstance = instance;

	return true;
}

void RenderWindow::Resize()
{
	RECT windowRect;
	GetClientRect(mWindowHandle, &windowRect);

	mWidth = windowRect.right - windowRect.left;
	mHeight = windowRect.bottom - windowRect.top;
}

RenderWindow::RenderWindow(RenderWindowDesc& desc)
	: Base()
	, mHInstance(desc.HInstance)
	, mParentHandle(desc.ParentHandle)
	, mWindowHandle(nullptr)
	, mTitle(StringConverter::ConvertStringToWide(desc.Title))
	, mClassName(StringConverter::ConvertStringToWide(desc.ClassName))
	, mWidth(desc.Width)
	, mHeight(desc.Height)
{
}

bool RenderWindow::initialize(Application* application)
{
	if (!registerWindowClass())
	{
		return false;
	}

	if (!createWindow(application))
	{
		return false;
	}

	ShowWindow(mWindowHandle, SW_SHOW);
	SetForegroundWindow(mWindowHandle);
	SetFocus(mWindowHandle);

	return true;
}

bool RenderWindow::registerWindowClass()
{
	WNDCLASSEX wc;
	wc.style = CS_HREDRAW | CS_VREDRAW;
	wc.lpfnWndProc = HandleMessageSetup;
	wc.cbClsExtra = 0;
	wc.cbWndExtra = 0;
	wc.hInstance = mHInstance;
	wc.hIcon = nullptr;
	wc.hIconSm = nullptr;
	wc.hCursor = LoadCursor(0, IDC_CROSS);
	wc.hbrBackground = nullptr;
	wc.lpszMenuName = nullptr;
	wc.lpszClassName = mClassName.c_str();
	wc.cbSize = sizeof(WNDCLASSEX);

	if (!RegisterClassExW(&wc))
	{
		ErrorLog("RegisterClassEx failed.");

		return false;
	}

	return true;
}
bool RenderWindow::createWindow(Application* application)
{
	RECT windowRect;
	windowRect.left = GetSystemMetrics(SM_CXSCREEN) / 2 - mWidth / 2;
	windowRect.top = GetSystemMetrics(SM_CYSCREEN) / 2 - mHeight / 2;
	windowRect.right = windowRect.left + mWidth;
	windowRect.bottom = windowRect.top + mHeight;
	DWORD windowStyle = WS_CHILD;
	AdjustWindowRect(&windowRect, windowStyle, false);

	mWindowHandle = CreateWindowExW(0, mClassName.c_str(), mTitle.c_str(), windowStyle, windowRect.left, windowRect.top, windowRect.right - windowRect.left, windowRect.bottom - windowRect.top, mParentHandle, nullptr, mHInstance, application);
	if (mWindowHandle == nullptr)
	{
		ErrorLog("CreateWindowEx failed.");

		return false;
	}

	return true;
}

void RenderWindow::destroy()
{
	if (mWindowHandle != nullptr)
	{
		UnregisterClassW(mClassName.c_str(), mHInstance);
		DestroyWindow(mWindowHandle);
	}
}