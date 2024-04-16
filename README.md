# Locker Capstones

## StreamingAssets Overview

### Directories

- attractMedia - directory that contains the media that appears in the "Attract" state for ALL the eras
- lockerLocatorMedia - directory that contains the media that appears in the "Locker Locator" display for ALL possible lockers
- mediaCache - directory where all media for this capstone's era is downloaded; is cleared if the era changes
- profileBackgrounds - directory that contains the media that appears in the upper half behind the "Menu" and "Profile" states for ALL the eras

### JSON

- config.json - configuration document for this capstone
- data.json - data cache from CMS for this capstone's era; is cleared if the era changes; is updated after a successful query to the CMS


## Config Overview

- eraID - integer UID for this capstone; corresponds to the "id" field for the era in the CMS
- chooseEraTimeout - integer timeout duration (seconds) for the "Setup" state;  after elapsing, the capstone continues with the chosen era
- timeout - integer timeout duration (seconds) for the capstone to return to the "Attract" state
- lockerLocatorMediaDirectoryName - string name of the directory where the media for the "Locker Locator" display is found; the capstone will search for this folder at the top of StreamingAssets/
- doLoadFromCMS - boolean; if false, the capstone will load data from its cache using data.json after the "Setup" state; if true, the capstone will attempt to query api server for the data after the "Setup" state
- apiServer - string base URL (e.g. ending in ".com") for GraphQL server
- apiToken - string basic auth token for GraphQL server
- apiEndpoint - string endpoint for URL for GraphQL server (e.g. "graphql"); will be joined to apiServer to form a full URL
- eraOptions - list of objects with details for each era; the data referenced by these fields is stored on ALL capstones and allows them to change eras during the "Setup" state

### Era Options Fields

- name - string name that appears during the "Setup" state; does NOT affect the visitor-facing name of the era stored in the CMS
- eraID - integer UID for this capstone; corresponds to the "id" field for the era in the CMS
- hex - string hex code that specifies the era-specific color for the capstone
- attractMediaDirectoryName - string subpath to attract media for the given capstone; the capstone will search for this folder at the top of StreamingAssets/
- profileBackgroundsDirectoryName - string subpath to profile backgrounds media for the given capstone; the capstone will search for this folder at the top of StreamingAssets/

## Locker Locator Media

### Filename Specification

Filenames should match the "Locker Number" field in the CMS e.g. "40411.png"

### File Size Specification

Images will be scaled to fit to a height of 1200px, with a resulting variable width. There is no padding within the container. The maximum visible width is the screen's, at 2160px. The image is centered within its container, which is also screen width.
