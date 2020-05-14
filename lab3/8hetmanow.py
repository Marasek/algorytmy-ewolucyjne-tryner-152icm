# -*- coding: cp1250 -*-
import datetime
import random
import statistics
import sys
import time


class Chromosome:
    def __init__(self, genes, fitness):
        self.Genes = genes
        self.Fitness = fitness

class Board:
    # konstruktor inicjujący szachownicę losowym położeniem hetmanów
    def __init__(self, genes, size):
        board = [['x'] * size for _ in range(size)]
        # konwersja string->intiger
        genes = [int(i) for i in genes]
        for index in range(0, len(genes), 2):
            row = genes[index]
            column = genes[index + 1]
            board[column][row] = 'D'
        self._board = board

    # rysowanie szachownicy z hetmanami
    def print(self):
        # 0,0 prints in bottom left corner
        for i in reversed(range(len(self._board))):
            print(' '.join(self._board[i]))
            
    # zwracamy konkretną pozycję hetmana
    def get(self, row, column):
        return self._board[column][row]

genes1 = [random.randint(0, 7) for i in range(16)]
print("Losowe współrzędne:", genes1)
board1 = Board(genes1, 8)
board1.print()

class Fitness:
    def __init__(self, total):
        self.Total = total

    def __gt__(self, other):
        return self.Total < other.Total
    
    def __ge__(self, other):
        return self.Total <= other.Total

    def __str__(self):
        return "{}".format(self.Total)

def get_fitness(genes, size):
    board = Board(genes, size)
    szeregDam = set()
    kolumnyDam = set()
    northeastprzekatneDam = set()
    southheastprzekatneDam = set()
    for row in range(size):
        for col in range(size):
            if board.get(row, col) == 'D':
                szeregDam.add(row)
                kolumnyDam.add(col)
                northeastprzekatneDam.add(row + col)
                southheastprzekatneDam.add(size - 1 - row + col)
    total = size - len(szeregDam) \
            + size - len(kolumnyDam) \
            + size - len(northeastprzekatneDam) \
            + size - len(southheastprzekatneDam)
    return Fitness(total)

def display(candidate, startTime, size):
    board = Board(candidate.Genes, size)
    board.print()
    print("Geny: {};\t fitness - {}. \t".format(
        ' '.join(map(str, candidate.Genes)),
        candidate.Fitness))

def get_best(get_fitness, targetLen, optimalFitness, geneSet, display):
    random.seed()
    bestParent = _generate_parent(targetLen, geneSet, get_fitness)
    display(bestParent)
    if bestParent.Fitness > optimalFitness:
        return bestParent
    
    # uruchom tylko na kilka sekund
    czas_dzialania = 3
    t_end = time.time() + czas_dzialania

    while time.time() < t_end:
        child = _mutate(bestParent, geneSet, get_fitness)
        if bestParent.Fitness > child.Fitness:
            continue
        display(child)
            
        if child.Fitness > optimalFitness:
            return child
        bestParent = child

def _generate_parent(length, geneSet, get_fitness):
    genes = []
    while len(genes) < length:
        sampleSize = min(length - len(genes), len(geneSet))
        genes.extend(random.sample(geneSet, sampleSize))
    # konwersja intiger->string
    genes = [str(i) for i in genes]

    genes = ''.join(genes)
    fitness = get_fitness(genes)
    return Chromosome(genes, fitness)

# mutacja
def _mutate(parent, geneSet, get_fitness):
    index = random.randrange(0, len(parent.Genes))
    childGenes = list(parent.Genes)
    newGene, alternate = random.sample(geneSet, 2)
    childGenes[index] = str(alternate) if str(newGene) == childGenes[index] else str(newGene)
    genes = ''.join(childGenes)
    fitness = get_fitness(genes)
    return Chromosome(genes, fitness)

def start(size=8):
        geneset = [i for i in range(size)]
        startTime = datetime.datetime.now()

        def fnDisplay(candidate):
            display(candidate, startTime, size)

        def fnGetFitness(genes):
            return get_fitness(genes, size)

        optimalFitness = Fitness(0)
        best = get_best(fnGetFitness, 2 * size, optimalFitness,
                                geneset, fnDisplay)
        
start(8)

