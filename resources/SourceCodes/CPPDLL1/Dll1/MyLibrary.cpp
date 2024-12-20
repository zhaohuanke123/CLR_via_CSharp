#include "pch.h"
#include <string>
#include "MyLibrary.h"


int __cdecl add(int a, int b) {
	return a + b;
}

const char* __cdecl getMessage1() {
	static std::string message = "SB C#, C++ is the best Coding Language";
	return message.c_str();
}

int* newPtr()
{
	printf("Enter newPtr\n");
	int* p = new int();
	return p;
}

void releasePtr(int* p)
{
	printf("Enter releasePtr\n");
	if (p != nullptr)
	{
		printf("Delete!!\n");
		delete p;
	}
}
