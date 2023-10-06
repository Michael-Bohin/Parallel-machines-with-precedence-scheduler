# Identical parellel machiens with precedence scheduler

Benchmark of different aproximation algorithms for P5|prec|Cmax.

There are 7 algorithms compared in the test. The first two are somewhat trivial. Third to seventh are adaptations of LPT (longest processing time first) algorithm that is known for simpler version of the problem Pm||Cmax. This simpler variant of the problem does not include the precedence of jobs.

Therefor the nature of the benchmark is to test various adaptations of known algorithms for the Pm||Cmax problem in the context of more complex P5|prec|Cmax problem. 

1. Naive. Searches all options in exponential time.
2. Sheduling by layers of sources.
3. LPT-sources
4. LPT-sourcePaths
5. LPT-allWithSourcePaths
6. LPT-allWithCriticalPaths
7. LCPT (longest critical path time first)
8. LCPC (longest critical path count first)

## Scheduling by layers of sources

0. initiate machines times
1. Create hashset Jobs of jobs to recompute
2. while Jobs is nonempty:
3. 		create empty list of jobs "layer"
4. 		foreach job j in Jobs:
5. 			if(all dependencies of j have finnished):
6. 				remove j from Jobs
7. 				add j to layer		
8.		foreach job k in layer
9. 			schedule k 
10. print output

## Naive

