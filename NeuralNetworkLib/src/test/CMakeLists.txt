project (testNeuralNetwork CXX)
set(PROJECTS ${PROJECTS} ${PROJECT_NAME} PARENT_SCOPE)
IF(WIN32)
set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /D _MBCS")
ENDIF()
set(SOURCES_FILES 
		./main.cpp
		)
add_executable(${PROJECT_NAME} ${SOURCES_FILES})
set_target_properties(${PROJECT_NAME} PROPERTIES DEBUG_POSTFIX "d")
target_link_libraries(${PROJECT_NAME} NeuralNetwork)
