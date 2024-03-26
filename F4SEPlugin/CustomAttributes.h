#pragma once

#include "Json.h"


namespace Data
{
	struct CustomAttributes
	{
		CustomAttributes() = default;
		CustomAttributes(CustomAttributes&& other) noexcept : data(std::move(other.data)){}
		CustomAttributes(Json::JObject data) noexcept : data(std::move(data)){}
		~CustomAttributes() = default;
		CustomAttributes(const CustomAttributes&) = delete;
		CustomAttributes& operator=(const CustomAttributes& other) = delete;
		CustomAttributes& operator=(CustomAttributes&& other) noexcept 
		{
			data = std::move(other.data);
			return *this;
		}
		std::string to_string() const
		{
			return data.to_string();
		}
	protected:
		Json::JObject data;
	};
}



