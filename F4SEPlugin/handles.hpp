#pragma once
#include <windows.h>

struct auto_handle
{
	auto_handle() = delete;
	auto_handle(auto_handle&&) = default;
	auto_handle(const auto_handle&) = delete;
	auto_handle(auto_handle&) = delete;
	auto_handle& operator= (auto_handle) = delete;
	auto_handle& operator= (auto_handle&&) = default;

	operator const HANDLE() const
	{
		return h;
	}

	auto_handle(HANDLE&& handle)
	{
		h = handle;
	}
	~auto_handle()
	{
		CloseHandle(h);
	}
	bool operator == (HANDLE handle) const
	{
		return h == handle;
	}
private:
	HANDLE h;
};
struct auto_map
{
	auto_map() = delete;
	auto_map(auto_map&&) = default;
	auto_map(const auto_map&) = delete;
	auto_map(auto_map&) = delete;
	auto_map& operator= (auto_map) = delete;
	auto_map& operator= (auto_map&&) = default;

	operator void*() const
	{
		return ptr;
	}

	auto_map(void* && other)
	{
		ptr = other;
	}
	~auto_map()
	{
		UnmapViewOfFile(ptr);
	}
	bool operator == (void* other) const
	{
		return ptr == other;
	}
private:
	void* ptr;
};