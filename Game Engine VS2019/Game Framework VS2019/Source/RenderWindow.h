#pragma once
#include "Base.h"

class Application;

struct RenderWindowDesc
{
	HINSTANCE HInstance;
	HWND ParentHandle;

	std::string Title;
	std::string ClassName;

	UINT Width;
	UINT Height;
};

class RenderWindow final : public Base
{
public:
	static bool Create(RenderWindow** outInstance, RenderWindowDesc& desc, Application* application);

	void Resize();

	inline HWND GetHandle() const;
	inline UINT GetWidth() const;
	inline UINT GetHeight() const;

private:
	RenderWindow() = delete;
	explicit RenderWindow(RenderWindowDesc& desc);
	virtual ~RenderWindow() override = default;

	bool initialize(Application* application);
	bool registerWindowClass();
	bool createWindow(Application* application);

	virtual void destroy() override;

private:
	HINSTANCE mHInstance;
	HWND mParentHandle;
	HWND mWindowHandle;

	std::wstring mTitle;
	std::wstring mClassName;

	UINT mWidth;
	UINT mHeight;
};

inline HWND RenderWindow::GetHandle() const
{
	return mWindowHandle;
}

inline UINT RenderWindow::GetWidth() const
{
	return mWidth;
}

inline UINT RenderWindow::GetHeight() const
{
	return mHeight;
}