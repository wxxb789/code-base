set(CMAKE_TOOLCHAIN_FILE "$ENV{VCPKG_ROOT}/scripts/buildsystems/vcpkg.cmake" CACHE STRING "Vcpkg toolchain file")

find_package(Catch2 3 REQUIRED)

# These tests can use the Catch2-provided main
add_executable(catch2 help.cpp)
target_link_libraries(catch2 PRIVATE Catch2::Catch2WithMain)