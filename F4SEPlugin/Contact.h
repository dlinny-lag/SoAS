#pragma once
#include "ContactArea.h"
#include <ostream>
#include <functional>

namespace Data
{
	enum class PainType
	{
		None = 0,
		Cold = 1,
		Hot = 2,
		Electro = 3,
		Acid = 4,
		Pierce = 5,
		Slash = 6,
		Crush = 7,
	};
	struct Contact
	{
		UInt32 ParticipantIndex;
		ContactArea Area;
		std::string ReversePath;
		SInt32 Stimulation;
		SInt32 Hold;
		SInt32 Pain;
		SInt32 Tickle;
		SInt32 Comfort;
		PainType PainType;

		void Dump(std::ostream& stream, const std::function<std::string(size_t)>& getParticipantTitle) const
		{
			stream << getParticipantTitle(ParticipantIndex) << "'s " << Area.Id;
			if (!ReversePath.empty())
				stream << "/" << ReversePath;
			stream << " (";
			stream << "stim=" << Stimulation;
			stream << ", hold=" << Hold;
			stream << ", pain(" << (int)PainType << ")=" << Pain; 
			stream << ", tickle=" << Tickle;
			stream << ", comfort=" << Comfort;
			stream << ")";
		}
		static const constexpr char* PathDelimiter = "/"; // same to SceneModel.ContactAreas.BodyPart.Delimiter
	};

	struct ActorsContact
	{
		Contact From;
		Contact To;
		void Dump(std::ostream& stream, const std::function<std::string(size_t)>& getParticipantTitle) const
		{
			stream << "Contact from ";
			From.Dump(stream, getParticipantTitle);
			stream << " to ";
			To.Dump(stream, getParticipantTitle);
		}
	};

	enum class Directions
    {
        FromEnvironment = 0,
        FromActor = 1,
    };

	struct EnvironmentContact
	{

		Directions Direction {};
		Contact Contact;
		void Dump(std::ostream& stream, const std::function<std::string(size_t)>& getParticipantTitle) const
		{
			stream << "Contact from ";
			if (Directions::FromEnvironment == Direction)
			{
				stream << " environment to ";
				Contact.Dump(stream, getParticipantTitle);
			}
			else
			{
				Contact.Dump(stream, getParticipantTitle);
				stream << " to environment";
			}
		}
	};
}

