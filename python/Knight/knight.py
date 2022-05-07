moveSet = [-17, 15, -10, 6, -6, 10, -15, 17]
locked = []
steps = 0

class colors:
    reset='\033[0m'
    class fg:
        black='\033[30m'
        white='\033[37m'
    class bg:
        black='\033[40m'
        red='\033[41m'
        green='\033[42m'
        white='\033[47m'

for i in range(64):
    locked.append(False)

def DebugOut():
    global steps
    print(f"--------------------------{steps}")

    for j in range(8):
        for i in range(8):
            if  (j % 2 == 0):
                if (i % 2 == 0): print(colors.bg.black, colors.fg.white, end="")
                else: print(colors.bg.white, colors.fg.black, end="")
            else:
                if (i % 2 == 0): print(colors.bg.white, colors.fg.black, end="")
                else: print(colors.bg.black, colors.fg.white, end="")
            if (locked[(j * 8) + i]): print(format((j * 8) + i, "0>2"), end="")
            else: print("  ", end="")
        print(colors.reset)

def CheckMoveValidity(parentPos, move):
    if (parentPos + move < 0): return False
    if (parentPos + move > 63): return False
    if (locked[parentPos + move]): return False
    if moveSet.index(move) // 4 == 0:
        return parentPos % 8 >= 8 - (move % 8)
    return parentPos % 8 <= 8 - (move % 8)

def GetValidMoves(parentPos):
    validMoves = []
    for m in moveSet:
        if (CheckMoveValidity(parentPos, m)): validMoves.append(m)
    return validMoves

def UnlockedCount():
    count = 0
    for x in locked:
        if (not x):
            count += 1
    return count

def RecursiveSearch(parentPos):
    global steps
    steps += 1
    locked[parentPos] = True
    DebugOut()
    if (UnlockedCount() == 0): return 0

    possibleMoves = GetValidMoves(parentPos)
    if (len(possibleMoves) == 0): return 1

    UnsortedEvalGroup = {}
    for p in possibleMoves:
        UnsortedEvalGroup[p] = len(GetValidMoves(parentPos + p))
    evalGroup = dict(sorted(UnsortedEvalGroup.items(),key= lambda x:x[1]))

    for e in evalGroup:
        if (RecursiveSearch(parentPos + e) == 0): return 0

    locked[parentPos] = False
    return 1

print("Input starting posistion between 0 and 63: ", end="")
startPos = int(input())
while (startPos < 0 or startPos > 63):
    print("Invalid input")
    print("Input starting posistion between 0 and 63: ", end="")
    startPos = int(input())

if (RecursiveSearch(startPos) == 0):
    print("success")
else:
    print("fail")