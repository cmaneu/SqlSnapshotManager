Sql Snapshot Service
==================

This tool and library helps you to manage SQL Server database snapshots.

# What inside ?

## 1. A .net API

The core project gives you a simple managed API to : 
  * Create a snapshot
  * Delete a snapshot
  * Restore a database from a snapshot
  * Enumerate all databases with a snapshot, and their snapshots

## 2. A REST Json/XML Api

Based on ASP.net Web API, you can simply make the same operations against your servers.
This API is preliminary, uses the account of the host (IIS, ...) and is **not** production-ready.

# Come on !

Feel free to fork this project, add what you want, and also push back to this repo ;).

# Credits
* [Christopher Maneu](http://maneu.net/) ([@cmaneu](http://twitter.com/cmaneu))
