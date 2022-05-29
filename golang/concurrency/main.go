package main

import (
	"context"
	"fmt"
	"io"
	"net/http"
	"os"
	"os/signal"
	"syscall"

	"golang.org/x/sync/errgroup"
)

func main() {

	g, ctx := errgroup.WithContext(context.Background())

	mux := http.NewServeMux()

	mux.HandleFunc("/hello", func(w http.ResponseWriter, r *http.Request) {
		io.WriteString(w, "Hello")
	})

	stop := make(chan struct{})
	mux.HandleFunc("/stop", func(w http.ResponseWriter, r *http.Request) {
		stop <- struct{}{}
	})

	server := http.Server{
		Addr:    "127.0.0.1:8080",
		Handler: mux,
	}

	g.Go(func() error {
		return server.ListenAndServe()
	})

	g.Go(func() error {
		signals := make(chan os.Signal, 1)

		signal.Notify(signals, syscall.SIGINT, syscall.SIGILL)

		select {
		case <-ctx.Done():
			return ctx.Err()
		case <-signals:
			return fmt.Errorf("get os exit %v", signals)
		}
	})

	g.Go(func() error {
		select {
		case <-ctx.Done():
			fmt.Println("ctx done")
		case <-stop:
			fmt.Println("stop")
		}

		return server.Shutdown(ctx)
	})

	err := g.Wait()
	fmt.Println(err)
}
