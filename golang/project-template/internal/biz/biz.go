package biz

import "wxxb789/code-snippets/golang/project-template/internal/data"

func NewUserBiz(userRepo data.UserRepository) *UserBiz {
	return &UserBiz{
		userRepo: userRepo,
	}
}
