#pragma once
#include <cctype>
#include <sstream>
#include <algorithm>
#include <vector>
#include <string_view>

struct SU
{
	static std::string ToUpper(const std::string& s)
	{
		// All tags are in ASCII, so we don't care about locale during case conversion
		std::string copy = s;
		std::transform(copy.begin(), copy.end(), copy.begin(), [](unsigned char c) { return std::toupper(c); });
		return copy;
	}
	static std::string ToUpper(const char* s)
	{
		// All tags are in ASCII, so we don't care about locale during case conversion
		std::string copy = s;
		std::transform(copy.begin(), copy.end(), copy.begin(), [](unsigned char c) { return std::toupper(c); });
		return copy;
	}

	static bool StartsFrom(const char* value, const char* pattern)
	{
		if (value == pattern)
			return true;

		if (!value || !pattern)
			return false; // null strings are not comparable

		do
		{
			if (!*pattern) // pattern finished, so if here then value starts from pattern. if pattern is empty, then any string matches the pattern
				return true;

			if (*value != *pattern)
				return false;

			++value;
			++pattern;
		}
		while(*value);
		return *value == *pattern;
	}

	static std::vector<std::string_view> Split(const char* value, const char* delimiter, bool skipEmpty = true)
	{
		std::vector<std::string_view> retVal;
		const char* current = value;
		const char* start = current;
		const size_t delimiterLength = strlen(delimiter);
		while (*current)
		{
			if (!StartsFrom(current, delimiter))
			{
				++current;
				continue;
			}
			if (start != current || !skipEmpty)
				retVal.emplace_back(std::string_view(start, current-start));
			current += delimiterLength;
			start = current;
		}
		if (start != current || !skipEmpty)
			retVal.emplace_back(std::string_view(start, current-start));
		return retVal;
	}


	template <typename Range, typename Value = typename Range::value_type>
	static std::string Join(Range const& elements, const char *const delimiter) // see https://stackoverflow.com/a/5289170
	{
	    std::ostringstream os;
	    auto b = begin(elements), e = end(elements);

	    if (b != e) 
		{
	        std::copy(b, prev(e), std::ostream_iterator<Value>(os, delimiter));
	        b = prev(e);
	    }
	    if (b != e) 
		{
	        os << *b;
	    }

	    return os.str();
	}
};