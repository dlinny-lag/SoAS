#pragma once
#include <string>
#ifndef SInt32
	#include "F4SETypeDefs.h"
#endif
namespace Data
{
	struct Sex final
	{
		const static Sex Any;
		const static Sex Male;
		const static Sex Female;
		operator size_t() const {return value;}
		operator std::string_view() const;
		Sex():value(-1){} // should cause exceptions
		Sex(const Sex& other) noexcept : value(other.value){}
		Sex(Sex&& other) noexcept : value(other.value) {other.value = -1;}
		~Sex() = default;
		Sex& operator = (const Sex& other) noexcept = default;
		Sex& operator = (Sex&& other) noexcept = default;
	private:
		UInt32 value;
		Sex(UInt32 val):value(val){}
	};
}