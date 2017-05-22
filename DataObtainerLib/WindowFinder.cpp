#include "stdafx.h"
#include "WindowFinder.h"

HWND WindowFinder::result;

WindowFinder::WindowFinder()
{
}

WindowFinder::~WindowFinder()
{
}

BOOL WindowFinder::Match(HWND hWnd, LPARAM lParam)
{
	LPWSTR title = new WCHAR[100];
	GetWindowText(hWnd, title, 100);
	LPWSTR part = (LPWSTR)lParam;
	if (std::wstring(title).find(std::wstring(part)) != std::string::npos)
	{
		result = hWnd;
		return false;
	}
	else
		return true;
}

HWND WindowFinder::FindWindowByText(HWND hwndParent, HWND hwndChildAfter, LPCTSTR lpszClass, LPCTSTR lpszWindow)
{
	result = NULL;
	EnumChildWindows(hwndParent, Match, (LPARAM)lpszWindow);
	return result;
}
