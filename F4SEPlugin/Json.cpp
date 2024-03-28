#include "Json.h"
using namespace Json;
JToken::JToken(const JToken& other) : Type(other.Type)
{
	switch (Type)
	{
		case TokenType::Null: AS.emplace<0>(std::monostate()); return;
		case TokenType::Object: AS.emplace<1>(std::unique_ptr<JObject>(other.AsObject()->Clone())); return;
		case TokenType::Array: AS.emplace<2>(std::unique_ptr<JArray>(other.AsArray()->Clone())); return;
		case TokenType::String: AS.emplace<3>(other.AsString()); return;
		case TokenType::Integer: AS.emplace<4>(other.AsInt()); return;
		case TokenType::Float: AS.emplace<5>(other.AsFloat()); return;
		case TokenType::Boolean: AS.emplace<6>(other.AsBool()); return;
		case TokenType::Property: break;
	}
	throw std::exception("Unsupported token type");
}

std::string JToken::to_string() const
{
	switch (Type)
	{
		case TokenType::Null: return "null";
		case TokenType::Object: return AsObject()->to_string();
		case TokenType::Array: return AsArray()->to_string();
		case TokenType::String: return "\"" + AsString() + "\"";
		case TokenType::Integer: return std::to_string(AsInt());
		case TokenType::Float: return std::to_string(AsFloat());
		case TokenType::Boolean: return AsBool()?"true":"false";
		case TokenType::Property: break;
	}
	throw std::exception("Unsupported token type");
}
