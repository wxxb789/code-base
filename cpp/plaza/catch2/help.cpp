#include <catch2/catch_test_macros.hpp>

#include <string>

std::string hello_world()
{
    return "hello_world";
}

TEST_CASE("hello_world", "[hello_world]")
{
    REQUIRE(hello_world() == "hello_world");
}