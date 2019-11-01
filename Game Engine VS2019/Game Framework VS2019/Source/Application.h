#pragma once
#include "Base.h"

class RenderWindow;
class GraphicDevice;

class Application abstract : public Base
{
public:
	Application();
	virtual ~Application() override = default;

	LRESULT WindowProc(HWND hWND, UINT uMsg, WPARAM wParam, LPARAM lParam) const;

	HWND GetWindowHandle() const;
	UINT GetWindowWidth() const;
	UINT GetWindowHeight() const;

	ID3D11Device* GetDevice() const;
	ID3D11DeviceContext* GetDeviceContext() const;

protected:
	virtual bool resize() = 0;

	virtual void destroy() override;

protected:
	RenderWindow* mRenderWindow;
	GraphicDevice* mGraphicDevice;
};