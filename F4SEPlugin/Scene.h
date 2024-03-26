#pragma once
#include <vector>
#include <sstream>
#include "Participant.h"
#include "Contact.h"
#include "CustomAttributes.h"

namespace Data
{
	inline std::ostream& operator << (std::ostream& os, const std::vector<std::string>& values)
	{
		bool added = false;
		for(const std::string& value : values)
		{
			if (added)
				os << ", ";
			added = true;
			os << value;
		}
		return os;
	}

	struct Scene
	{
		enum SceneType
		{
			None = 0,
			Single = 1,
			Sequence = 2,
			Tree = 3,
		};

		std::string Id;
		SceneType Type;
		std::vector<std::string> Furniture;
		std::vector<Participant> Participants;
		std::vector<ActorsContact> ActorsContacts;
		std::vector<EnvironmentContact> EnvironmentContacts;

		std::vector<std::string> Tags;
		std::vector<std::string> Authors;
		std::vector<std::string> Narrative;
		std::vector<std::string> Feeling;
		std::vector<std::string> Service;
		std::vector<std::string> Attribute;
		std::vector<std::string> Other;
		CustomAttributes Custom;

		void DumpParticipantTitle(std::ostream& stream, size_t index) const
		{
			const auto& p = Participants[index];
			stream << "#" << index + 1 << " ";
			stream << p.Skeleton << "["  << static_cast<std::string_view>(p.Sex) << "]";
		}

		void DumpParticipant(std::ostream& stream, size_t index) const
		{
			DumpParticipantTitle(stream, index);
			stream << ": ";
			// attributes:
			const auto& p = Participants[index];
			bool added = false;
			if (p.IsAggressor)
			{
				stream << "aggressor";
				added = true;
			}
			if (p.IsVictim)
			{
				if (added)
					stream << ", ";
				stream << "victim";
			}
		}

		[[nodiscard]] std::string GetParticipantTitle(size_t index) const
		{
			std::stringstream stream;
			DumpParticipantTitle(stream, index);
			return stream.str();
		}

		void Dump(std::ostream& stream) const
		{
			stream << Id << std::endl;
			stream << Type << std::endl;
			stream << "Authors: " << Authors << std::endl;
			stream << "Furniture: " << Furniture << std::endl;
			stream << "Tags: " << Tags << std::endl;
			stream << "Custom Attributes: " << Custom.to_string() << std::endl;
			for(size_t i = 0; i < Participants.size(); i++)
			{
				DumpParticipant(stream, i);
				stream << std::endl;
			}
			const std::function getTitle = [this](size_t index){return GetParticipantTitle(index);};
			for(size_t i = 0; i < ActorsContacts.size(); i++)
			{
				ActorsContacts[i].Dump(stream, getTitle);
			}
			for(size_t i = 0; i < EnvironmentContacts.size(); i++)
			{
				EnvironmentContacts[i].Dump(stream, getTitle);
			}
			stream << std::endl;
		}

	};

}
