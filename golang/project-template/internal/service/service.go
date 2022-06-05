package service

import "wxxb789/code-snippets/golang/project-template/internal/biz"

func NewUserService(biz *biz.UserBiz) *UserService {

	return &UserService{
		userBiz: biz,
	}
}
