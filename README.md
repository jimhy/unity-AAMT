# AAMT(Auto-Assets-Manager-Tools) Unity自动化资源管理工具

### 当前版本 v0.1.0

### 讨论QQ群:85968052

### 功能

- 自动管理、打包Bundle
- 自动热更管理
- 自动加载管理(支持多种不同加载方式)
- 自动资源释放管理

---

### 版本历史

#### v0.2.0 `2022/3/6`

- [X]  SetingManager bundle打包到StreammigAssets文件夹下，并且加载的时候也需要到StreamigAssets文件夹中去加载。
- [ ]  增加移动需要到StreamingAssets文件加的路径列表。
- [ ]  需要把assets-info.txt改成ScriptableObject文件，一起打包到aamt.ab里面。
- [ ]  需要把移动到StreamingAssets文件夹的文件记录一个文件列表文件，供移动功能使用。
- [ ]  需要加一个是否移动文件到StreammigAssets文件夹下的勾选框，并写移动文件逻辑。
- [ ]  需要加一个多加载路径的功能，把加载路径更改成路径列表并实现功能。
- [ ]  实现热更文件对比，并且下载热更文件逻辑。

#### v0.1.0 `2022/3/5`

- 完成管理Bundle功能、打包功能
- 完成加载管理功能(支持多种不同加载方式)
- 完成Bundle引用计数自动释放功能
