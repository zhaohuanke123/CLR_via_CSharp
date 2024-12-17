#pragma once

// 导出符号定义
#ifdef MYLIBRARY_EXPORTS
#define MYLIBRARY_API __declspec(dllexport)
#else
#define MYLIBRARY_API __declspec(dllimport)
#endif

extern "C" {
    MYLIBRARY_API int __cdecl  add(int a, int b);
    MYLIBRARY_API const char* __cdecl getMessage1();
    MYLIBRARY_API int* _cdecl testPtr(int*);
}
