#include <iostream>
#include <string>
#include <mutex>
#include <thread>
#include <atomic>
#include <chrono>
#include <vector>

using namespace std;

class ThreadSafeArrayIncrementer
{
private:
	int numTasks = 300;
	int g_index = 0;
	mutex g_index_mutex;
	atomic_int a_index = { 0 };

	vector<uint8_t> mas;

	void initialize(int numTasks)
	{
		mas = vector<uint8_t>(numTasks, 0);
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
				mas.at(g_index)++;
				g_index++;
				g_index_mutex.unlock();
				//std::this_thread::sleep_for(std::chrono::nanoseconds(10));
			}
			else
			{
				g_index_mutex.unlock();
				break;
			}
			
		}
		
	}

	void task_atomic()
	{

		while (true)
		{
			auto index = a_index.fetch_add(1);
			//std::cout << std::this_thread::get_id() << '\n';
			if (index < numTasks)
			{
				mas.at(index)++;
				//std::this_thread::sleep_for(std::chrono::nanoseconds(10));
			}
			else break;
			
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




