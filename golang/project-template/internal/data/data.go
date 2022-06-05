package data

import (
	"gorm.io/driver/mysql"
	"gorm.io/gorm"
)

func NewDB() (db *gorm.DB) {

	dsn := "<<<db connection string>>>"

	db, _ = gorm.Open(mysql.Open(dsn), &gorm.Config{})

	return db

}
