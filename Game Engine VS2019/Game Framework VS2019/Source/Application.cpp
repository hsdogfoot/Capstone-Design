#include "stdafx.h"
#include "Application.h"

#include "GraphicDevice.h"
#include "RenderWindow.h"

Application::Application()
	: Base()
	, mRenderWindow(nullptr)
	, mGraphicDevice(nullptr)
{
}

LRESULT Application::WindowProc(HWND hWND, UINT uMsg, WPARAM wParam, LPARAM lParam) const
{
	return DefWindowProc(hWND, uMsg, wParam, lParam);
}

HWND Application::GetWindowHandle() const
{
	return mRenderWindow->GetHandle();
}

UINT Application::GetWindowWidth() const
{
	return mRenderWindow->GetWidth();
}

UINT Application::GetWindowHeight() const
{
	return mRenderWindow->GetHeight();
}

ID3D11Device* Application::GetDevice() const
{
	return mGraphicDevice->GetDevice();
}

ID3D11DeviceContext* Application::GetDeviceContext() const
{
	return mGraphicDevice->GetDeviceContext();
}

void Application::destroy()
{
	ReleaseBase(mRenderWindow);
	ReleaseBase(mGraphicDevice);
}
