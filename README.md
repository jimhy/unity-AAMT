# AAMT(Auto-Assets-Manager-Tools) Unity自动化资源管理工具

### 当前版本 v0.1.0

### 讨论QQ群:85968052

### 功能

- 自动管理、打包Bundle
- 自动热更管理
- 自动加载管理(支持多种不同加载方式)
- 自动资源释放管理

---

### 未来功能

- [ ]  断点续传功能

### 版本历史

#### v0.2.0 `2022/3/8`

- [X]  SetingManager bundle打包到StreammigAssets文件夹下，并且加载的时候也需要到StreamigAssets文件夹中去加载。
- [X]  增加打包apk时需要移动到StreamingAssets文件夹的路径列表。
- [X]  需要把移动到StreamingAssets文件夹的文件记录一个文件列表文件。
- [X]  增加Setting文件中，需要把ab移动到StreamingAssets文件夹下的目录列表，一起打包到apk的功能。
- [X]  第一次启动游戏时,把文件移动到PersistentDataPath文件夹中。
- [X]  实现远程下载AB功能
- [ ]  在SettingManager里面加上eidtor,window,android,ios等平台的配置文件保存属性
- [ ]  加载AB时，需要首先在PersistentDataPath文件夹中找，如果找不到，就到StreamingAssets文件中找，如果找不到，就到远程目录下下载下来(需要勾选)。
- [ ]  实现热更文件对比，并且下载热更文件逻辑。

#### v0.1.0 `2022/3/5`

- 完成管理Bundle功能、打包功能
- 完成加载管理功能(支持多种不同加载方式)
- 完成Bundle引用计数自动释放功能
