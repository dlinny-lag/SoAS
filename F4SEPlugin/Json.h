#pragma once
#include <memory>
#include <unordered_map>
#include <variant>
#include <sstream>
#ifndef SInt32
	#include "F4SETypeDefs.h"
#endif

namespace Json
{
	enum class TokenType // same as Newtonsoft.Json.Linq.JTokenType ( https://www.newtonsoft.com/json/help/html/t_newtonsoft_json_linq_jtokentype.htm )
	{
		Object = 1,
		Array = 2,
		Property = 4,
		Integer = 6,
		Float = 7,
		String = 8,
		Boolean = 9,
		Null = 10,
	};

	struct JObject;
	struct JArray;
	struct JToken final
	{
	private:
		TokenType Type;
		std::variant<std::monostate, std::unique_ptr<JObject>, std::unique_ptr<JArray>, std::string, SInt32, float, bool> AS;
	public:
		JToken() noexcept : Type(TokenType::Null), AS(std::monostate()){}
		JToken(std::unique_ptr<JObject> obj) noexcept : Type(TokenType::Object), AS(std::move(obj)){}
		JToken(std::unique_ptr<JArray> arr) noexcept : Type(TokenType::Array), AS(std::move(arr)){}
		JToken(SInt32 value) noexcept : Type(TokenType::Integer), AS(value){}
		JToken(std::string value) noexcept : Type(TokenType::String), AS(value){}
		JToken(float value) noexcept : Type(TokenType::Float), AS(value){}
		JToken(bool value) noexcept : Type(TokenType::Boolean), AS(value){}
		~JToken() = default;

		JToken(const JToken& other);
		JToken& operator=(const JToken& other) = delete;
		void swap(JToken& other) noexcept
		{
			const TokenType otherType = other.Type;
			auto otherData = std::move(other.AS);
			other.Type = Type;
			other.AS = std::move(AS);
			Type = otherType;
			AS = std::move(otherData);
		}
		JToken(JToken&& other) noexcept : Type(other.Type), AS(std::move(other.AS)) {other.Type = TokenType::Null;}
		JToken& operator=(JToken&& other) noexcept
		{
			Type = other.Type;
			other.Type = TokenType::Null;
			AS = std::move(other.AS);
			return *this;
		}

		[[nodiscard]] TokenType GetType() const noexcept {return Type;}

		[[nodiscard]] bool IsNull() const noexcept {return TokenType::Null == Type;}
		[[nodiscard]] bool IsObject() const noexcept {return TokenType::Object == Type;}
		[[nodiscard]] bool IsArray() const noexcept {return TokenType::Array == Type;}
		[[nodiscard]] bool IsString() const noexcept {return TokenType::String == Type;}
		[[nodiscard]] bool IsInt() const noexcept {return TokenType::Integer == Type;}
		[[nodiscard]] bool IsFloat() const noexcept {return TokenType::Float == Type;}
		[[nodiscard]] bool IsBool() const noexcept {return TokenType::Boolean == Type;}

		[[nodiscard]] const JObject * AsObject() const {return std::get<1>(AS).get();}
		[[nodiscard]] const JArray * AsArray() const {return std::get<2>(AS).get();}
		[[nodiscard]] const std::string& AsString() const {return std::get<3>(AS);}
		[[nodiscard]] SInt32 AsInt() const {return std::get<4>(AS);}
		[[nodiscard]] float AsFloat() const {return std::get<5>(AS);}
		[[nodiscard]] bool AsBool() const {return std::get<6>(AS);}

		[[nodiscard]] std::string to_string() const;
	};


	struct JObject final
	{
		std::unordered_map<std::string, JToken> Properties;
		[[nodiscard]] JObject * Clone() const
		{
			return new JObject{Properties};
		}
		[[nodiscard]] std::string to_string() const
		{
			std::stringstream ss;
			ss << "{";
			bool added = false;
			for(const auto& pair : Properties)
			{
				if (added)
					ss << ", ";
				added = true;

				ss << "\"" << pair.first << "\":";
				ss << pair.second.to_string();
			}
			
			ss << "}";
			return ss.str();
		}
	};
	struct JArray final
	{
		std::vector<JToken> Elements;
		[[nodiscard]] JArray * Clone() const
		{
			return new JArray{Elements};
		}
		[[nodiscard]] std::string to_string() const
		{
			std::stringstream ss;
			ss << "[";
			bool added = false;
			for(const auto& element : Elements)
			{
				if (added)
					ss << ", ";
				added = true;
				ss << element.to_string();
			}
			ss << "]";
			return ss.str();
		}
	};
}
