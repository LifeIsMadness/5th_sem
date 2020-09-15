#include <iostream>
#include <string>
#include <mutex>
#include <thread>
#include <atomic>
#include <chrono>

using namespace std;

class ThreadSafeArrayIncrementer
{
private:
	int numTasks = 300;
	int g_index = 0;
	mutex g_index_mutex;
	atomic_int a_index = 0;

	uint8_t *mas = NULL;

	void initialize(int numTasks)
	{
		mas = new uint8_t[numTasks];
		for (int i = 0; i < numTasks; i++) mas[i] = 0;
	}

	void task_mutex()
	{
		//const lock_guard<mutex> lock(g_index_mutex);
		
		while (g_index < numTasks)
		{
			g_index_mutex.lock();
			//std::cout << std::this_thread::get_id() << '\n';
			if (g_index < numTasks)
			{
				mas[g_index]++;
				g_index_mutex.unlock();
				//std::this_thread::sleep_for(std::chrono::nanoseconds(10));
			}
			else
			{
				g_index_mutex.unlock();
				break;
			}
			g_index++;
			
		}
		
	}

	void task_atomic()
	{
		while (a_index < numTasks)
		{
			//std::cout << std::this_thread::get_id() << '\n';
			if (a_index < numTasks)
			{
				mas[a_index]++;
				//std::this_thread::sleep_for(std::chrono::nanoseconds(10));
			}
			else break;
			a_index++;
		}
	}

public:

	ThreadSafeArrayIncrementer(int numTasks)
	{
		this->numTasks = numTasks;
		initialize(numTasks);
	}

	~ThreadSafeArrayIncrementer()
	{
		delete [] mas;
	}

	void run_threads_mutex(size_t numThreads = 2)
	{
		thread* threads = new thread[numThreads];

		for (size_t i = 0; i < numThreads; i++)
		{
			threads[i] = thread(&ThreadSafeArrayIncrementer::task_mutex, this);
		}

		getExecutionTime(numThreads, threads);
		
		//showArray();
		delete[] threads;
	}

	void run_threads_atomic(size_t numThreads = 2)
	{
		thread* threads = new thread[numThreads];
		for (size_t i = 0; i < numThreads; i++)
		{
			threads[i] = thread(&ThreadSafeArrayIncrementer::task_atomic, this);
		}

		getExecutionTime(numThreads, threads);
		//showArray();
		delete[] threads;
	}

	void getExecutionTime(const size_t& numThreads, std::thread* threads)
	{
		auto begin = std::chrono::steady_clock::now();
		for (size_t i = 0; i < numThreads; i++)
		{
			threads[i].join();
		}
		auto end = std::chrono::steady_clock::now();

		auto elapsed_ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - begin);
		std::cout << "The time: " << elapsed_ms.count() << " ms\n";
	}

	void showArray()
	{
		for (int i = 0; i < numTasks; i++) cout << (int)mas[i] << ' ';
		cout << endl << "=====================" << endl;
	}
};

int main()
{
	ThreadSafeArrayIncrementer incrementer(1024 * 1024);
	incrementer.run_threads_mutex(4);
	incrementer.run_threads_atomic(4);

	return 0;
}




