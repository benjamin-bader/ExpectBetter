#!/bin/sh -x

# Just a script to run unit tests

nuget install NUnit.Runners -Version 2.6.3 -o packages

runTest() {
  mono --runtime=v4.0.30319 packages/NUnit.Runners.2.6.3/tools/nunit-console.exe -noxml -nodots -labels $@
  if [ $? -ne 0 ]
  then
    exit 1
  fi
}

runTest $1 -exclude=Performace

exit $?
