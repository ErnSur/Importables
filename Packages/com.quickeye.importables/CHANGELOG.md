
# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## [2.0.0] - 2023-10-28

Fixes, improvements and more tests

### Added
- More Tests

### Changed
- Removed `UnityObjectContractResolver.UnityObjectConverter` property

### Fixed

- Subassets are now correctly serialized by `EditorUnityObjectConverter`
- Collections of Unity Objects are now correctly serialized by `EditorUnityObjectConverter` and `RuntimeUnityObjectConverter`

## [1.0.1] - 2023-09-13

Some bugfixes

### Added

### Changed

### Fixed

- NRE in `EditableAssetTargetView` OnDestroy
- Added missing `TextFilePreview.Cleanup` call for Unity 2021+