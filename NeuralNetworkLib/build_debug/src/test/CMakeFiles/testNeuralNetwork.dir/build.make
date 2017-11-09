# CMAKE generated file: DO NOT EDIT!
# Generated by "Unix Makefiles" Generator, CMake Version 3.0

#=============================================================================
# Special targets provided by cmake.

# Disable implicit rules so canonical targets will work.
.SUFFIXES:

# Remove some rules from gmake that .SUFFIXES does not remove.
SUFFIXES =

.SUFFIXES: .hpux_make_needs_suffix_list

# Suppress display of executed commands.
$(VERBOSE).SILENT:

# A target that is always out of date.
cmake_force:
.PHONY : cmake_force

#=============================================================================
# Set environment variables for the build.

# The shell in which to execute make rules.
SHELL = /bin/sh

# The CMake executable.
CMAKE_COMMAND = /opt/ISTAR/TPL/COS7U1/x86_64/neoCore/bin/cmake

# The command to remove a file.
RM = /opt/ISTAR/TPL/COS7U1/x86_64/neoCore/bin/cmake -E remove -f

# Escaping for special characters.
EQUALS = =

# The top-level source directory on which CMake was run.
CMAKE_SOURCE_DIR = /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll

# The top-level build directory on which CMake was run.
CMAKE_BINARY_DIR = /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll/build_debug

# Include any dependencies generated for this target.
include src/test/CMakeFiles/testNeuralNetwork.dir/depend.make

# Include the progress variables for this target.
include src/test/CMakeFiles/testNeuralNetwork.dir/progress.make

# Include the compile flags for this target's objects.
include src/test/CMakeFiles/testNeuralNetwork.dir/flags.make

src/test/CMakeFiles/testNeuralNetwork.dir/main.cpp.o: src/test/CMakeFiles/testNeuralNetwork.dir/flags.make
src/test/CMakeFiles/testNeuralNetwork.dir/main.cpp.o: ../src/test/main.cpp
	$(CMAKE_COMMAND) -E cmake_progress_report /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll/build_debug/CMakeFiles $(CMAKE_PROGRESS_1)
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Building CXX object src/test/CMakeFiles/testNeuralNetwork.dir/main.cpp.o"
	cd /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll/build_debug/src/test && /opt/ISTAR/TPL/COS7U1/x86_64/neoCore/bin/g++   $(CXX_DEFINES) $(CXX_FLAGS) -o CMakeFiles/testNeuralNetwork.dir/main.cpp.o -c /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll/src/test/main.cpp

src/test/CMakeFiles/testNeuralNetwork.dir/main.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/testNeuralNetwork.dir/main.cpp.i"
	cd /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll/build_debug/src/test && /opt/ISTAR/TPL/COS7U1/x86_64/neoCore/bin/g++  $(CXX_DEFINES) $(CXX_FLAGS) -E /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll/src/test/main.cpp > CMakeFiles/testNeuralNetwork.dir/main.cpp.i

src/test/CMakeFiles/testNeuralNetwork.dir/main.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/testNeuralNetwork.dir/main.cpp.s"
	cd /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll/build_debug/src/test && /opt/ISTAR/TPL/COS7U1/x86_64/neoCore/bin/g++  $(CXX_DEFINES) $(CXX_FLAGS) -S /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll/src/test/main.cpp -o CMakeFiles/testNeuralNetwork.dir/main.cpp.s

src/test/CMakeFiles/testNeuralNetwork.dir/main.cpp.o.requires:
.PHONY : src/test/CMakeFiles/testNeuralNetwork.dir/main.cpp.o.requires

src/test/CMakeFiles/testNeuralNetwork.dir/main.cpp.o.provides: src/test/CMakeFiles/testNeuralNetwork.dir/main.cpp.o.requires
	$(MAKE) -f src/test/CMakeFiles/testNeuralNetwork.dir/build.make src/test/CMakeFiles/testNeuralNetwork.dir/main.cpp.o.provides.build
.PHONY : src/test/CMakeFiles/testNeuralNetwork.dir/main.cpp.o.provides

src/test/CMakeFiles/testNeuralNetwork.dir/main.cpp.o.provides.build: src/test/CMakeFiles/testNeuralNetwork.dir/main.cpp.o

# Object files for target testNeuralNetwork
testNeuralNetwork_OBJECTS = \
"CMakeFiles/testNeuralNetwork.dir/main.cpp.o"

# External object files for target testNeuralNetwork
testNeuralNetwork_EXTERNAL_OBJECTS =

bin/testNeuralNetworkd: src/test/CMakeFiles/testNeuralNetwork.dir/main.cpp.o
bin/testNeuralNetworkd: src/test/CMakeFiles/testNeuralNetwork.dir/build.make
bin/testNeuralNetworkd: bin/libNeuralNetwork.so
bin/testNeuralNetworkd: src/test/CMakeFiles/testNeuralNetwork.dir/link.txt
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --red --bold "Linking CXX executable ../../bin/testNeuralNetworkd"
	cd /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll/build_debug/src/test && $(CMAKE_COMMAND) -E cmake_link_script CMakeFiles/testNeuralNetwork.dir/link.txt --verbose=$(VERBOSE)

# Rule to build all files generated by this target.
src/test/CMakeFiles/testNeuralNetwork.dir/build: bin/testNeuralNetworkd
.PHONY : src/test/CMakeFiles/testNeuralNetwork.dir/build

src/test/CMakeFiles/testNeuralNetwork.dir/requires: src/test/CMakeFiles/testNeuralNetwork.dir/main.cpp.o.requires
.PHONY : src/test/CMakeFiles/testNeuralNetwork.dir/requires

src/test/CMakeFiles/testNeuralNetwork.dir/clean:
	cd /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll/build_debug/src/test && $(CMAKE_COMMAND) -P CMakeFiles/testNeuralNetwork.dir/cmake_clean.cmake
.PHONY : src/test/CMakeFiles/testNeuralNetwork.dir/clean

src/test/CMakeFiles/testNeuralNetwork.dir/depend:
	cd /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll/build_debug && $(CMAKE_COMMAND) -E cmake_depends "Unix Makefiles" /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll/src/test /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll/build_debug /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll/build_debug/src/test /0/jloquet/pers/machine_learning/projet_visual/NeuralNetworkDll/build_debug/src/test/CMakeFiles/testNeuralNetwork.dir/DependInfo.cmake --color=$(COLOR)
.PHONY : src/test/CMakeFiles/testNeuralNetwork.dir/depend

