import Data.List
import System.IO
import System.Directory 

main = do
    -- get the file contents a string.
    content <- (readFile "C:\\Users\\Merlijn\\Documents\\GitHub\\functioneel-programmeren-2\\input\\1.txt")
    -- convert it to an array with each line in it.
    let contentLines = lines content
    -- get the dimensions.
    let xSize = length (head contentLines)
    let ySize = length contentLines
    -- Initialize nodes
    let nodes = [[[x,y,if(contentLines!!y)!!x=='#' then -1 else 0]| x <- [0..xSize-1]] | y <- [0..ySize-1]]
    -- Set start and finish node
    let start  = (nodes!!0)!!0
    let finish = (nodes!!(ySize-1))!!(xSize-1)
    -- assign the nodes
    let assignedNodes = assignValues start finish nodes
    -- print the nodes
    putStrLn (intercalate "\n" ( [(intercalate "" ["["++(if (node!!2) < 9 then "0" else "" )++show((node!!2)+1)++"]" | node <- (assignedNodes!!y)]) | y <- [0..ySize-1]]))

    
assignValues :: [Int] -> [Int] -> [[[Int]]] -> [[[Int]]]
assignValues start finish nodes = assignValuesRecurisvly finish 1 [start] nodes

--finish, value, currentNodes, result
assignValuesRecurisvly :: [Int] -> Int  -> [[Int]] -> [[[Int]]] ->  [[[Int]]]
assignValuesRecurisvly _ _ [] result = result
assignValuesRecurisvly finish value currentNodes result  = 
    assignValuesRecurisvly 
        finish 
        (value+1) 
        (nub [nextNode | currNode <- currentNodes, nextNode <- getNeighbours currNode (length (head result)) (length result) result, nextNode!!2 == 0])
        [[[node!!0,node!!1,if (node `elem` currentNodes && node!!2 == 0) then value else node!!2] | node <- (result!!y)] | y <- [0..((length result)-1)]]
    
getNeighbours :: [Int] -> Int -> Int -> [[[Int]]] -> [[Int]]
getNeighbours pos xSize ySize nodes = [[x, y, nodes!!y!!x!!2] | 
        x <- [((pos!!0)-1)..((pos!!0)+1)], 
        y <- [((pos!!1)-1)..((pos!!1)+1)], 
        (x+y == (pos!!0) + (pos!!1) - 1 || x+y == (pos!!0) + (pos!!1) + 1) &&
        x >= 0 && x < xSize &&
        y >= 0 && y < ySize
    ]

traversePath :: [[[Int]]] -> [Int] -> [[Int]] -> [[Int]]
traversePath = _ path 1 = path
traversePath = nodes path currentValue = 
    traversePath
    
    (currentValue-1)

