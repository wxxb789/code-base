# CMakeList.txt : Top-level CMake project file, do global configuration and include sub-projects here.

cmake_minimum_required(VERSION 3.28.0 FATAL_ERROR)
set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED YES)
set(CMAKE_CXX_EXTENSIONS NO)

# Enable module scanning
set(CMAKE_EXPERIMENTAL_CXX_MODULE_DYNDEP 1)

# setup vcpkg
# include(FetchContent)
# FetchContent_Declare(
# vcpkg
# GIT_REPOSITORY https://github.com/Microsoft/vcpkg.git
# GIT_TAG 2023.08.09
# )
# FetchContent_MakeAvailable(vcpkg)
set(CMAKE_TOOLCHAIN_FILE "$ENV{VCPKG_ROOT}/scripts/buildsystems/vcpkg.cmake" CACHE STRING "Vcpkg toolchain file")

project("plaza")

# Enable Hot Reload for MSVC compilers if supported.
if(POLICY CMP0141)
  cmake_policy(SET CMP0141 NEW)
  set(CMAKE_MSVC_DEBUG_INFORMATION_FORMAT "$<IF:$<AND:$<C_COMPILER_ID:MSVC>,$<CXX_COMPILER_ID:MSVC>>,$<$<CONFIG:Debug,RelWithDebInfo>:EditAndContinue>,$<$<CONFIG:Debug,RelWithDebInfo>:ProgramDatabase>>")
endif()

# Include sub-projects.
add_subdirectory("plaza")
add_subdirectory("catch2")
