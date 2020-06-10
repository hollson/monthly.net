all: build

## build@编译应用。
.PHONY: build
build:
	@dotnet build ./Monthly

## run@启动测试。
.PHONY:test
test:
	@echo "启动测试用例..."
	@dotnet test

## clean@清理编译、日志和缓存等数据。
.PHONY:clean
clean: 
	@rm -rf ./.vs
	@rm -rf ./.vscode
	@rm -rf ./.DS_Store
	@rm -rf ./Monthly/obj
	@rm -rf ./Monthly/bin
	@rm -rf ./Monthly.Test/obj
	@rm -rf ./Monthly.Test/bin

## help@查看make帮助。
.PHONY:help
help:Makefile
	@echo "Usage:\n  make [command]"
	@echo
	@echo "Available Commands:"
	@sed -n "s/^##//p" $< | column -t -s '@' |grep --color=auto "^[[:space:]][a-z]\+[[:space:]]"
	@echo
	@echo "For more to see https://makefiletutorial.com/"