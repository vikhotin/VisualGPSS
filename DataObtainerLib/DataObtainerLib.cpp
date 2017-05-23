// DataObtainerLib.cpp: определяет экспортированные функции для приложения DLL.
//

#include "stdafx.h"
#include "WindowFinder.h"

wchar_t*** Alloc3DArrayWideChar(int size1, int size2, int size3);
void Free3DArray(wchar_t*** arr, int size1, int size2);

extern "C" HWND __declspec(dllexport) __stdcall FindGPSS()
{
	HWND gpss = WindowFinder::FindWindowByText(NULL, NULL, NULL, TEXT("GPSS World"));
	return gpss;
}

extern "C" HWND __declspec(dllexport) __stdcall FindBlocksWindow(HWND gpss)
{
	HWND mdicli = FindWindowEx(gpss, NULL, L"MDIClient", NULL);
	HWND blocks = WindowFinder::FindWindowByText(mdicli, NULL, NULL, TEXT("BLOCK ENTITIES"));
	return blocks;
}

extern "C" HWND __declspec(dllexport) __stdcall FindSimDataLV(HWND blocks)
{
	HWND listview = FindWindowEx(blocks, NULL, TEXT("SysListView32"), NULL);
	return listview;
}

extern "C" int __declspec(dllexport) __stdcall GetListviewCount(HWND listview)
{
	return (int)SendMessage(listview, LVM_GETITEMCOUNT, 0, 0);
}

extern "C" __declspec(dllexport) wchar_t*** __stdcall GetSimulationDataArray(const HWND listview)
{
	const int STR_MAX_SIZE = 24;

	int count = (int)SendMessage(listview, LVM_GETITEMCOUNT, 0, 0);
	int i;

	LVITEM lvi, *_lvi;
	wchar_t*** table = Alloc3DArrayWideChar(count, 4, STR_MAX_SIZE);
	wchar_t *_loc, *_type, *_curcount, *_entcount;

	unsigned long pid;
	HANDLE process;

	GetWindowThreadProcessId(listview, &pid);
	process = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ | 
						  PROCESS_VM_WRITE | PROCESS_QUERY_INFORMATION, FALSE, pid);

	_lvi = (LVITEM*)VirtualAllocEx(process, NULL, sizeof(LVITEM), MEM_COMMIT, PAGE_READWRITE);
	_loc = (wchar_t*)VirtualAllocEx(process, NULL, STR_MAX_SIZE, MEM_COMMIT, PAGE_READWRITE);
	_type = (wchar_t*)VirtualAllocEx(process, NULL, STR_MAX_SIZE, MEM_COMMIT, PAGE_READWRITE);
	_curcount = (wchar_t*)VirtualAllocEx(process, NULL, STR_MAX_SIZE, MEM_COMMIT, PAGE_READWRITE);
	_entcount = (wchar_t*)VirtualAllocEx(process, NULL, STR_MAX_SIZE, MEM_COMMIT, PAGE_READWRITE);

	lvi.cchTextMax = STR_MAX_SIZE;

	for (i = 0; i<count; i++) {
		lvi.iSubItem = 0;
		lvi.pszText = _loc;
		WriteProcessMemory(process, _lvi, &lvi, sizeof(LVITEM), NULL);
		SendMessage(listview, LVM_GETITEMTEXT, (WPARAM)i, (LPARAM)_lvi);

		lvi.iSubItem = 1;
		lvi.pszText = _type;
		WriteProcessMemory(process, _lvi, &lvi, sizeof(LVITEM), NULL);
		SendMessage(listview, LVM_GETITEMTEXT, (WPARAM)i, (LPARAM)_lvi);

		lvi.iSubItem = 2;
		lvi.pszText = _curcount;
		WriteProcessMemory(process, _lvi, &lvi, sizeof(LVITEM), NULL);
		SendMessage(listview, LVM_GETITEMTEXT, (WPARAM)i, (LPARAM)_lvi);

		lvi.iSubItem = 3;
		lvi.pszText = _entcount;
		WriteProcessMemory(process, _lvi, &lvi, sizeof(LVITEM), NULL);
		SendMessage(listview, LVM_GETITEMTEXT, (WPARAM)i, (LPARAM)_lvi);

		ReadProcessMemory(process, _loc, table[i][0], STR_MAX_SIZE, NULL);
		ReadProcessMemory(process, _type, table[i][1], STR_MAX_SIZE, NULL);
		ReadProcessMemory(process, _curcount, table[i][2], STR_MAX_SIZE, NULL);
		ReadProcessMemory(process, _entcount, table[i][3], STR_MAX_SIZE, NULL);
	}

	VirtualFreeEx(process, _lvi, 0, MEM_RELEASE);
	VirtualFreeEx(process, _loc, 0, MEM_RELEASE);
	VirtualFreeEx(process, _type, 0, MEM_RELEASE);
	VirtualFreeEx(process, _curcount, 0, MEM_RELEASE);
	VirtualFreeEx(process, _entcount, 0, MEM_RELEASE);

	return table;
}

extern "C" void __declspec(dllexport) __stdcall ClearData(wchar_t*** table, int count)
{
	Free3DArray(table, count, 4);
}

wchar_t*** Alloc3DArrayWideChar(int size1, int size2, int size3)
{
	wchar_t*** res = new wchar_t**[size1];
	for (int i = 0; i < size1; i++)
	{
		res[i] = new wchar_t*[size2];
		for (int j = 0; j < size2; j++)
		{
			res[i][j] = new wchar_t[size3];
		}
	}
	return res;
}

void Free3DArray(wchar_t*** arr, int size1, int size2)
{
	for (int i = 0; i < size1; i++)
	{
		for (int j = 0; j < size2; j++)
		{
			delete[] arr[i][j];
		}
		delete[] arr[i];
	}
	delete[] arr;
}