#pragma once
#include "Contact.h"
#include "StringUtils.hpp"

struct ContactFlag
{
	static bool Is(const Data::Contact& contact, const char* isExpression, bool& invalidExpression)
	{
		const constexpr char* prologue = "IS_";
		constexpr size_t prologueLength = 3;

		const std::string expr = SU::ToUpper(isExpression);
		if (expr.rfind(prologue, 0) != 0)
		{
			invalidExpression = true;
			return false;
		}
		const std::vector<std::string_view> expressionParts = SU::Split(expr.c_str() + prologueLength, "_");
		return Is(SU::ToUpper(contact.Area.Id), SU::Split(SU::ToUpper(contact.ReversePath).c_str(), Data::Contact::PathDelimiter), expressionParts);
	}

	// all arguments must be in upper case
	static bool Is(const std::string& area, std::vector<std::string_view> reversePath, const std::vector<std::string_view>& expressionParts)
	{
		size_t searchingPart = expressionParts.size()-1;
		std::string_view searchFor = expressionParts[searchingPart];
		reversePath.insert(reversePath.begin(), area);
		for(const auto part : reversePath)
		{
			bool needRepeat = false;
			do
			{
				if (TryFind(part, searchFor, searchingPart, expressionParts, needRepeat))
					return true;
			}
			while(needRepeat);
		}
		return false;
	}
private:
	static bool TryFind(const std::string_view part, std::string_view& searchFor, size_t& searchingPart, const std::vector<std::string_view>& expressionParts, bool& needRepeat)
	{
		needRepeat = false;
		if (part.rfind(searchFor) != std::string_view::npos)
		{
			if (searchingPart == 0)
				return true;
			--searchingPart;
			searchFor = expressionParts[searchingPart];
			needRepeat = true;
			return false;
		}
		return false;
	}
};
