# bank-task
Used operators:
	select
	join
	group by
	having
	where
	order by
	sub-queries
	limit
	offset
Interesting cases:
	with optional filters, where clause will if clause or param is null
	when retrieving top customers, grouping by id and name is required to retrive them both, although they are same
	with suspicious transactions, subquery is used to find id's of suspicious customers (by grouping and joining), then transactions joined with accounts of these customers from the last 1o minutes are retrieved
	