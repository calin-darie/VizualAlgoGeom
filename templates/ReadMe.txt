A common pattern has emerged for all algorithms. The AlgorithmAdapter Visual Studio template encapsulates this pattern to provide is a quick way to get started with algorithm development:. 
The template contains:
- an AlgorithmAdapter implementing the IAlgorithmAdapter interface
- sample resource files to define pseudocode, static & dynamic explanations, and visual styles
- the code to load the resource files 
- sample usage of the snapshot recorder

To install this template:
1. Create a zip archive from the files inside the AlgorithmAdapter folder. *Do not* zip the folder itself, but all of its contents.
2. Locate the custom project templates folder. In Visual Studio, navigate to Tools > Options > Projects and Solutions. 
The folder you're interested in is labeled "User project templates location:"
3. Copy the zip generated at step 1 to the subfolder Visual C# of the folder located at step 2.

After installing the template, for each new algorithm:
1. File > New project...
2. Choose AlgorithmAdapter
3. Enter the algorithm name - just the name, suffixes are automatically added to the namespaces, classes and generated assemblies.
4. Select the VizualAlgoGeom\Algorithms as the folder
5. Make sure that "Create directory for solution" is checked. Relative paths included in the template depend on this, plus it will make the 
new algorithm consistent with the old ones. 