#!/bin/bash
if [ -z ${CmdletSessionID} ]
then
  export CmdletSessionID=$PPID
fi
SCRIPTPATH=$(dirname "$0")
$SCRIPTPATH/az -s az -r $SCRIPTPATH/azure.lx "$@" 
