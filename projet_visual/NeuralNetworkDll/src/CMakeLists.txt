include_directories(../Include
					../include/eigen3
					)
add_subdirectory(DLL)
add_subdirectory(TestDLL)

set(PROJECTS ${PROJECTS} PARENT_SCOPE)
