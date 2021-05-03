package main

import (
	"fmt"
	"io/ioutil"
	"strings"
	"time"

	"github.com/ahmetb/go-linq/v3"
)

var grid [][]*node

var xSize int
var ySize int

var start *node
var finish *node

func main() {
	now := time.Now().UnixNano() / int64(time.Millisecond)
	initializeNodes()
	assignValues()
	path := traversePath()
	println((time.Now().UnixNano() / int64(time.Millisecond)) - now)

	printGrid(path)
}

func initializeNodes() {

	content, _ := ioutil.ReadFile("../input/1.txt")

	lines := strings.Split(string(content), "\n")
	// minus one since go counts line break as a character
	xSize = len(lines[0]) - 1
	ySize = len(lines)

	grid = [][]*node{}

	for y := 0; y < ySize; y++ {
		row := []*node{}
		for x := 0; x < xSize; x++ {
			row = append(row, &node{
				X:     x,
				Y:     y,
				Value: map[bool]int{true: -1, false: 0}[string(lines[y][x]) == "#"],
			})
		}
		grid = append(grid, row)
	}

	start = grid[0][0]
	finish = grid[ySize-1][xSize-1]
}

func assignValues() {
	currentNodes := []*node{start}

	assignValuesRecursivly(1, currentNodes)
}

func assignValuesRecursivly(value int, currentNodes []*node) {
	if len(currentNodes) == 0 || linq.From(currentNodes).Contains(finish) {
		finish.Value = value
		return
	}

	nextNodes := []*node{}

	for _, currentNode := range currentNodes {
		currentNode.Value = value

		neighbours := []*node{}

		for _, neighbour := range getNeighbours(currentNode.X, currentNode.Y) {
			if neighbour.Value == 0 {
				neighbours = append(neighbours, neighbour)
			}
		}

		nextNodes = append(nextNodes, neighbours...)
	}

	linq.From(nextNodes).Distinct().ToSlice(&nextNodes)

	assignValuesRecursivly(value+1, nextNodes)
}

func getNeighbours(x int, y int) []*node {

	result := []*node{}

	coords := [][]int{
		{+1, +0},
		{-1, +0},
		{+0, +1},
		{+0, -1},
	}

	for i := 0; i < len(coords); i++ {
		_x := coords[i][0] + x
		_y := coords[i][1] + y

		if _x >= 0 && _y >= 0 && _x < xSize && _y < ySize {
			result = append(result, grid[_y][_x])
		}

	}
	return result
}

func traversePath() []*node {

	path := []*node{finish}

	for path[len(path)-1].Value > 1 {

		lastNode := path[len(path)-1]

		for _, n := range getNeighbours(lastNode.X, lastNode.Y) {

			if n.Value == lastNode.Value-1 {
				path = append(path, n)
				break
			}
		}
	}
	return path
}

func printGrid(path []*node) {
	for y := 0; y < ySize; y++ {
		for x := 0; x < xSize; x++ {
			currentNode := grid[y][x]
			if linq.From(path).Contains(currentNode) {
				fmt.Print(".")
			} else if currentNode.Value == -1 {
				fmt.Print("#")
			} else {
				fmt.Print(" ")
			}

		}
		fmt.Println()
	}
}

type node struct {
	X     int
	Y     int
	Value int
}
