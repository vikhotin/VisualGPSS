#pragma once

class WindowFinder
{
public:
	WindowFinder();
	~WindowFinder();

private:
	static HWND result;

	static BOOL CALLBACK Match(HWND hWnd, LPARAM lParam);

public:
	static HWND FindWindowByText(
		_In_opt_ HWND    hwndParent,
		_In_opt_ HWND    hwndChildAfter,
		_In_opt_ LPCTSTR lpszClass,
		_In_opt_ LPCTSTR lpszWindow);
};
