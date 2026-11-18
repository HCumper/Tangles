namespace Tangles.Generator

type DifficultyTarget = AnyDifficulty | Easy | Medium | Hard | Expert

module GenerationPolicy =
    type Options =
        { Target: DifficultyTarget
          MaxCandidates: int
          ImproveSteps: int
          SolverWork: int
          MinTrials: int
          MinBranchDepth: int
          MinFirstSolutionNodes: int
          Assess: bool
          RequireUnique: bool
          AvoidShortest: bool }

    let defaults =
        { Target = AnyDifficulty; MaxCandidates = 100; ImproveSteps = 100
          SolverWork = Solver.defaults.MaxWork; MinTrials = 0; MinBranchDepth = 0; MinFirstSolutionNodes = 0
          Assess = false; RequireUnique = false; AvoidShortest = false }

    let targetName = function AnyDifficulty -> "any" | Easy -> "easy" | Medium -> "medium" | Hard -> "hard" | Expert -> "expert"
    let active options = options.Assess || options.Target <> AnyDifficulty || options.RequireUnique || options.AvoidShortest
    let strengthened options = options.MinTrials > 0 || options.MinBranchDepth > 0 || options.MinFirstSolutionNodes > 0
    let depthSelection options = options.MinBranchDepth > 0 || options.MinFirstSolutionNodes > 0
    let meetsMinimums options (analysis: Solver.Analysis) =
        analysis.Status = "complete"
        && analysis.TrialSteps >= options.MinTrials
        && analysis.FirstSolutionBranchDepth >= options.MinBranchDepth
        && analysis.FirstSolutionSearchNodes >= options.MinFirstSolutionNodes

    type Statistics =
        { Algorithm: string
          Target: string
          CandidateLimit: int
          CandidatesConsidered: int
          CandidatesEvaluated: int
          TraversalEvaluations: int
          ImprovementLimit: int
          SolverWorkLimit: int
          MinTrials: int
          MinBranchDepth: int
          MinFirstSolutionNodes: int
          Assess: bool
          RequireUnique: bool
          AvoidShortest: bool
          DuplicatesRejected: int
          BudgetRejections: int
          StartingTier: string
          StartingTrialSteps: int }

    type Report =
        { Reasoning: Solver.Analysis option
          ShortestRoutes: Solver.ShortestRouteScreen option
          Selection: Statistics }
