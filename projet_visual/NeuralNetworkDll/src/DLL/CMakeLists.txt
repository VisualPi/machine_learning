project (NeuralNetwork CXX)

set(PROJECTS ${PROJECTS} ${PROJECT_NAME} PARENT_SCOPE)

set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /DDLL_EXPORT /D_WINDLL /D_USRDLL")
set(SOURCES_FILES	LinearModel.cpp
					PerceptronMulticoucheModel.cpp
					RBFModel.cpp
					)
add_library(${PROJECT_NAME} SHARED ${SOURCES_FILES})