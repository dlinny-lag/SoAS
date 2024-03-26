#include "Sex.h"

const Data::Sex Data::Sex::Any(0);
const Data::Sex Data::Sex::Male(1);
const Data::Sex Data::Sex::Female(2);

std::string_view map[3]{"P", "M", "F"};
Data::Sex::operator std::string_view() const
{
	return map[value];
}

