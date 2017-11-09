include_directories(
			../include
			../include/eigen3
			)
add_subdirectory(lib)
add_subdirectory(test)

set(PROJECTS ${PROJECTS} PARENT_SCOPE)
