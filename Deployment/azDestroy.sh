#!/bin/bash
#
# Deletes the resource group and everything in it
#

# Make sure we're in the right subscription
az account set -s aicoe.dev.test
az group delete -n cruze-test-group
