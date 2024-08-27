# Visualized Sorting

## Description
This project is meant as a tool to visualize some of the more well-known sorting algorithms. The current prototype is pretty barebones and not very user-friendly. But we plan to expand on the idea in the near future.

## Content
The algorithms we have implemented so far are the following:
- Bubble Sort 
- Insertion Sort
- Selection Sort
- Quick Sort
- Merge Sort
- Heap Sort

## Usage

### Installation
To play with the current build, you can clone the repository or download it as a .zip. The latest build can be found as an executable in the folder "Builds". If you're not into running unknown .exe files from internet strangers, you can instead open the project in the Unity editor.

### Controls
Everything is controlled with the keyboard, as we haven't implemented a proper UI yet. But the bindings are pretty easy to understand. The *spacebar* is used to scramble an already sorted array and to stop any currently running algorithm. And the bindings to run the algorithms are their initials, so 'B' for Bubble Sort, 'Q' for Quick Sort, and so forth. 

So, in summary:
- **Spacebar** | Pause/scramble
- **B** | Run Bubble Sort
- **I** | Run Insertion Sort
- **S** | Run Selection Sort
- **Q** | Run Quick Sort
- **M** | Run Merge Sort
- **H** | Run Heap Sort

## Future Plans
- User-friendly UI 
- Colorization and sound for the visualizations 
- Explanations and theory for each of the algorithms 
- Pseudocode for each algorithm, both with and without explanatory comments
- A way to play with different pivot selection strategies for Quick Sort
- Real-time graphing of time complexity