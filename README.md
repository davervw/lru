# Last Recently Used Exercise #

lrucs is a C# code example of a sample LRU class

A last recently used cache is a common implementation for code and hardware.  For example, cache memory for storage devices and CPU memory access.  Caches are designed to be lightning quick and provide access to much needed data trying to avoid slower accesses to the underlying hardware and/or communication channel.  When present in the cache, should return the data in constant time O(1).  When commonly used resources are available quicker, perforance of the system will be greatly improved.  LRU benefits that if a resource is used often enough, the data will remain in the cache.

Examples for algorithms include database lookups of commonly accessed information, including joined or computational intensive data.  A real-world example from my past was accessing messages based on message id which were implemented in an overlay not guaranteed to be loaded into memory (primitive alternative to virtual memory); so by implementing an LRU cache on top of the retrieve message API this greatly improved the user experience because disk (floppy!) accesses were more limited.  Today the comparison would be to an over the network request to an HTTPS and/or SQL database to retrieve information for a data driven user experience.  Caching could also reduce real-world costs associated with cloud API metering.

The included code was drafted to complete the exercise of a functioning LRU class.  Additional work could be made to refactor and improve it, more refinement into a polished quality work.

Notes

0. Did NOT yet search for LRU definition or examples on web.  So views are mostly mine and could have faulty assumptions.
1. Stack overflow and Google, Microsoft helped with some not null syntax, and understanding of compiler errors (nullable stuff mostly)
2. Used IDE (Visual Studio) warnings, suggestions, refactoring tools
3. Hmmm, maybe could this be replaced with a priority queue?   Would that make it trivial?
4. What C# interfaces should this class instantiate to be generally useful?
5. Improve tests, more expectations validated, and change to automated tests instead of manual run.  Received direction/hints that test-driven development is preferred and agree.
6. Could enhance to implement a more transparent interface to a more expensive accessor such as web service, database or disk, so accessor gets from LRU cache if present, otherwise from slower device if there was a cache miss.  Beyond that could implement the remaining CRUD interfaces.
7. I was originally surprised two maps/dictionaries were required in this implementation, but makes sense in retrospect.  The data is ordered both by time of last access (simulated by a 64-bit unsigned), and by key, so makes sense.  Originally thought the keys could be a list/array, but finding them in that collection would require iterating over the whole collection, so moved it to another map/dictionary.
8. In retrospect I dived into the implementation way too fast.  Need to slow down, document the requirements, tests and interface first, then worry about the implementation.
