#!/bin/bash
#
# Creates a resource group and all the necessary components for TVS In a Box
#
az account set -s aicoe.dev.test

# Create the resource group with appropriate tags
az group create \
     -g cruze-test-group \
     -l westus2 \
     --tags contact=mike.lorengo@alaskaair.com InUse=True

# Create the appservice plan for all the webapps
az appservice plan create \
    -n cruze-test-plan \
    -g cruze-test-group \
    -l westus2 \
    --sku S1 

# Create the webapp for the api
az webapp create \
    -n cruzeapi-test-app \
    -g cruze-test-group \
    -p cruze-test-plan

# List all the outbound ip addresses
az webapp show \
    -n cruzeapi-test-app \
    -g cruze-test-group \
    --query outboundIpAddresses \
    -o tsv