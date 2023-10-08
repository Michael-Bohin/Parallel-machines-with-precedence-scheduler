<style>
        .green {
            color: rgb(78, 201, 176);
        }

        .lblue {
            color: rgb(156, 220, 254);
        }

        .dblue {
            color: rgb(77, 156, 214);
        }

        <span class="green"></span>
        <span class="lblue"></span>
        <span class="dblue"></span>
</style>

# Identical parellel machines with precedence scheduler

Benchmark of different aproximation algorithms for P5|prec|Cmax problem.

There are 8 algorithms compared in the test. The first two are somewhat trivial. Third to eight are adaptations of LPT (longest processing time first) algorithm that is known for simpler version of the problem Pm||Cmax. This simpler variant of the problem does not include the precedence constraint of jobs.

Therefor the nature of the benchmark is to test various adaptations of known algorithms for the Pm||Cmax problem in the context of more complex P5|prec|Cmax problem. 

1. Naive. Searches all options in exponential time.
2. Scheduling by layers of sources.
3. LPT-sources
4. LPT-sourcePaths
5. LPT-allWithSourcePaths
6. LPT-allWithCriticalPaths
7. LCPT (longest critical path time first)
8. LCPC (longest critical path count first)

## Scheduling by layers of sources

Input: <span class="green">List</span>\<<span class="green">Job</span>> <span class="lblue">jobs</span>, <span class="green">HashSet</span>\<<span class="dblue">int</span>> <span class="lblue">toRecompute</span>



## Naive

